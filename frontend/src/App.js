import React, { useState } from "react";
import "./App.css";

function App() {
  const [query, setQuery] = useState("");
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);

  const searchAPI = async (searchQuery) => {
    if (!searchQuery.trim()) {
      setResults([]);
      return;
    }

    setLoading(true);
    try {
      const response = await fetch(
        `http://localhost:5000/api/autocomplete?query=${searchQuery}`
      );
      const data = await response.json();
      setResults(data);
    } catch (error) {
      console.error("Error fetching results:", error);
      setResults([]);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const value = e.target.value;
    setQuery(value);
    searchAPI(value);
  };

  const highlightMatch = (text, query) => {
    if (!text || !query.trim()) return text;
    const regex = new RegExp(`(${query})`, "gi");
    const parts = text.split(regex);
    return parts.map((part, index) =>
      regex.test(part) ? <mark key={index}>{part}</mark> : part
    );
  };

  const groupedResults = results.reduce((acc, item) => {
    const type = item.Type || item.type;
    if (!acc[type]) acc[type] = [];
    acc[type].push(item);
    return acc;
  }, {});

  return (
    <div className="App">
      <div className="search-container">
        <h1>Autocomplete Search</h1>
        <input
          type="text"
          className="search-box"
          placeholder="Search for stocks or experts..."
          value={query}
          onChange={handleInputChange}
        />

        {loading && <div className="loading">Searching...</div>}

        {query && !loading && results.length === 0 && (
          <div className="results">
            <div className="no-results">No results found</div>
          </div>
        )}

        {Object.keys(groupedResults).length > 0 && (
          <div className="results">
            {groupedResults.stock && (
              <div className="result-group">
                <h3>Stocks</h3>
                <div className="result-header stock-item">
                  <div className="ticker">Ticker</div>
                  <div className="name">Name</div>
                  <div className="market-cap">Market Cap</div>
                </div>
                {groupedResults.stock.map((item, index) => (
                  <div key={index} className="result-item stock-item">
                    <div className="ticker">
                      {highlightMatch(item.Ticker, query)}
                    </div>
                    <div className="name">
                      {highlightMatch(item.Name, query)}
                    </div>
                    <div className="market-cap">
                      ${(item.MarketCap / 1e9).toFixed(2)}B
                    </div>
                  </div>
                ))}
              </div>
            )}

            {groupedResults.expert && (
              <div className="result-group">
                <h3>Experts</h3>
                <div className="result-header expert-item">
                  <div className="name">Name</div>
                  <div className="expert-type">Type</div>
                </div>
                {groupedResults.expert.map((item, index) => (
                  <div key={index} className="result-item expert-item">
                    <div className="name">
                      {highlightMatch(item.Name, query)}
                    </div>
                    <div className="expert-type">{item.ExpertType}</div>
                  </div>
                ))}
              </div>
            )}
          </div>
        )}
      </div>
    </div>
  );
}

export default App;
